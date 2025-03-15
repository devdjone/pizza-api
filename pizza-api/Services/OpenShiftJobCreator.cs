  


using k8s;
using k8s.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pizza_api.Services
{
    public class OpenShiftJobCreator
    {
        private readonly Kubernetes _client;

        public OpenShiftJobCreator()
        {
            var config = KubernetesClientConfiguration.InClusterConfig(); // Use this if running inside Kubernetes
            _client = new Kubernetes(config);
        }

        public async Task CreateAsync(string campaignId)
        {
            var jobName = $"campaign-processor-{campaignId}";
            var jobExists = await JobExistsAsync(jobName);
            if (jobExists)
            {
                Console.WriteLine($"Job {jobName} already exists.");
                return;
            }

            // Define the OpenShift Job resource dynamically
            var job = new V1Job
            {
                ApiVersion = "batch/v1",
                Kind = "Job",
                Metadata = new V1ObjectMeta
                {
                    Name = jobName,
                    NamespaceProperty = "pizza-app",
                    Labels = new Dictionary<string, string>
                    {
                        { "app.kubernetes.io/part-of", "sms-campaign-processors" },
                        { "campaignGroup", $"campaign-{campaignId}" }
                    }
                },
                Spec = new V1JobSpec
                {
                    Template = new V1PodTemplateSpec
                    {
                        Metadata = new V1ObjectMeta
                        {
                            Labels = new Dictionary<string, string>
                            {
                                { "app.kubernetes.io/part-of", "sms-campaign-processors" },
                                { "campaignGroup", $"campaign-{campaignId}" }
                            }
                        },
                        Spec = new V1PodSpec
                        {
                            RestartPolicy = "Never", // Ensures job runs once per execution
                            Containers = new List<V1Container>
                            {
                                new V1Container
                                {
                                    Name = "campaign-processor",
                                    Image = "image-registry.openshift-image-registry.svc:5000/pizza-app/sms-campaign-app",
                                    Env = new List<V1EnvVar>
                                    {
                                        new V1EnvVar { Name = "CAMPAIGN_ID", Value = campaignId }
                                    },
                                    Ports = new List<V1ContainerPort>
                                    {
                                        new V1ContainerPort { ContainerPort = 8080, Protocol = "TCP" }
                                    },
                                    ReadinessProbe = new V1Probe
                                    {
                                        SuccessThreshold = 1,
                                        TcpSocket = new V1TCPSocketAction { Port = 8080 }
                                    },
                                    SecurityContext = new V1SecurityContext
                                    {
                                        AllowPrivilegeEscalation = false,
                                        Capabilities = new V1Capabilities { Drop = new List<string> { "ALL" } },
                                        RunAsNonRoot = true,
                                        SeccompProfile = new V1SeccompProfile { Type = "RuntimeDefault" }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            await _client.CreateNamespacedJobAsync(job, "pizza-app");
        }

        private async Task<bool> JobExistsAsync(string jobName)
        {
            try
            {
                var job = await _client.ReadNamespacedJobAsync(jobName, "pizza-app");
                return job != null;
            }
            catch (Exception)
            {
                return false; // Job does not exist
            }
        }
    }
}
