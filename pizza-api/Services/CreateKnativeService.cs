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

            // Create the Knative Service
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
