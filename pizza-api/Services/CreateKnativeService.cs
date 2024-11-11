using k8s;
using k8s.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pizza_api.Services
{
    public class CreateKnativeService
    {
        public async Task CreateAsync(string campaignId)
        {
            var config = KubernetesClientConfiguration.InClusterConfig(); // Use this if running inside Kubernetes
            var client = new Kubernetes(config);

            // Define the Knative Service resource
            var knativeService = new V1CustomResourceDefinition
            {
                ApiVersion = "serving.knative.dev/v1",
                Kind = "Service",
                Metadata = new V1ObjectMeta
                {
                    Name = $"campaign-processor-{campaignId}",
                    NamespaceProperty = "default"
                },
                Spec = new V1CustomResourceDefinitionSpec
                {
                    Group = "serving.knative.dev",
                    Names = new V1CustomResourceDefinitionNames
                    {
                        Plural = "services",
                        Kind = "Service"
                    },
                    Versions = new List<V1CustomResourceDefinitionVersion>
                    {
                        new V1CustomResourceDefinitionVersion
                        {
                            Name = "v1",
                            Served = true,
                            Storage = true
                        }
                    }
                }
            };

            // Create the Knative Service s sdfsd 
            await client.CreateNamespacedCustomObjectAsync(
                knativeService,
                "serving.knative.dev",
                "v1",
                "default",
                "services",
                null);
        }



        public async Task CreateAsync2(string campaignId)
        {
            var config = KubernetesClientConfiguration.InClusterConfig(); // Use this if running inside Kubernetes
            var client = new Kubernetes(config);

            // Define the Knative Service resource as a dynamic object
            var knativeService = new
            {
                apiVersion = "serving.knative.dev/v1",
                kind = "Service",
                metadata = new
                {
                    name = $"campaign-processor-{campaignId}",
                    @namespace = "default"
                },
                spec = new
                {
                    template = new
                    {
                        metadata = new
                        {
                            annotations = new Dictionary<string, string>
                    {
                        { "autoscaling.knative.dev/minScale", "1" },
                        { "autoscaling.knative.dev/maxScale", "3" }
                    }
                        },
                        spec = new
                        {
                            containers = new[]
                            {
                        new
                        {
                            name = "campaign-processor",
                            image = "your-docker-registry/campaign-processor:latest",
                            env = new[]
                            {
                                new { name = "CAMPAIGN_ID", value = campaignId }
                            }
                        }
                    }
                        }
                    }
                }
            };

            // Use CreateNamespacedCustomObjectAsync with the dynamic Knative service
            await client.CreateNamespacedCustomObjectAsync(
                knativeService,
                "serving.knative.dev",
                "v1",
                "default",
                "services",
                null);
        }

    }




}
