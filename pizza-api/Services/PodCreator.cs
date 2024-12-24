using k8s;
using k8s.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pizza_api.Services
{

    public class PodCreator
    {
        private readonly Kubernetes _client;

        public PodCreator()
        {
            // This loads the default Kubernetes config, assuming the app is running within a Kubernetes environment.
            var config = KubernetesClientConfiguration.BuildDefaultConfig();
            _client = new Kubernetes(config);
        }

        public async Task CreatePodAsync(string campaignId, string kafkaTopic)
        {
            var pod = new V1Pod
            {
                ApiVersion = "v1",
                Kind = "Pod",
                Metadata = new V1ObjectMeta
                {
                    Name = $"campaign-processor-{campaignId}",
                    NamespaceProperty = "pizza-app"  // Specify your namespace here
                },
                Spec = new V1PodSpec
                {
                    Containers = new List<V1Container>
                    {
                        new V1Container
                        {
                            Name = "campaign-processor",
                            Image = "image-registry.openshift-image-registry.svc:5000/pizza-app/sms-campaign-app",
                            Env = new List<V1EnvVar>
                            {
                                new V1EnvVar { Name = "CAMPAIGN_ID", Value = campaignId },
                                new V1EnvVar { Name = "KAFKA_TOPIC", Value = kafkaTopic }  // Pass Kafka topic name as an environment variable
                            },
                            Ports = new List<V1ContainerPort>
                            {
                                new V1ContainerPort { ContainerPort = 8080, Protocol = "TCP" }
                            },
                            ReadinessProbe = new V1Probe
                            {
                                SuccessThreshold = 1,
                                // Define TcpSocket directly inside V1Probe
                                TcpSocket = new V1TCPSocketAction
                                {
                                    Port = 8080 // Specify the port as an IntOrString
                                }
                            },
                            Resources = new V1ResourceRequirements(),
                            SecurityContext = new V1SecurityContext
                            {
                                AllowPrivilegeEscalation = false,
                                Capabilities = new V1Capabilities
                                {
                                    Drop = new List<string> { "ALL" }
                                },
                                RunAsNonRoot = true,
                                SeccompProfile = new V1SeccompProfile
                                {
                                    Type = "RuntimeDefault"
                                }
                            }
                        }
                    }
                }
            };

            try
            {
                // Create the pod in the 'pizza-app' namespace
                var createdPod = await _client.CreateNamespacedPodAsync(pod, "pizza-app");
                Console.WriteLine($"Pod created: {createdPod.Metadata.Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating pod: {ex.Message}");
            }
        }

        private async Task<bool> ServiceExistsAsync(string serviceName)
        {
            try
            {
                // Check if the service exists in the specified namespace
                var service = await _client.ReadNamespacedServiceAsync(serviceName, "pizza-app");
                return service != null;
            }
            catch (Exception)
            {
                return false;  // Return false if the service does not exist
            }
        }

    }

    }
