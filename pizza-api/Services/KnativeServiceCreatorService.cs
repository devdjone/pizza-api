﻿
using k8s;
using k8s.Models;
using System;
using System.Collections.Generic; 
using System.Threading.Tasks;


namespace pizza_api.Services
{
public class KnativeServiceCreator
{
    private readonly Kubernetes _client;

    public KnativeServiceCreator()
    {
        var config = KubernetesClientConfiguration.InClusterConfig(); // Use this if running inside Kubernetes
        _client = new Kubernetes(config);
    }

    public async Task CreateAsync(string campaignId)
    {
        // Define the Knative Service resource dynamically
        var knativeService = new
        {
            apiVersion = "serving.knative.dev/v1",
            kind = "Service",
            metadata = new
            {
                name = $"campaign-processor-{campaignId}",
                @namespace = "pizza-app"
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
                                image = "image-registry.openshift-image-registry.svc:5000/pizza-app/sms-campaign-app",
                                env = new[]
                                {
                                    new { name = "CAMPAIGN_ID", value = campaignId }
                                },
                                ports = new[]
                                {
                                    new { containerPort = 8080, protocol = "TCP" }
                                },
                                readinessProbe = new
                                {
                                    successThreshold = 1,
                                    tcpSocket = new { port = 8080 }
                                },
                                resources = new { }, // You can add specific resource limits if necessary
                                securityContext = new
                                {
                                    allowPrivilegeEscalation = false,
                                    capabilities = new { drop = new[] { "ALL" } },
                                    runAsNonRoot = true,
                                    seccompProfile = new { type = "RuntimeDefault" }
                                }
                            }
                        }
                    }
                },
                traffic = new[]
                {
                    new
                    {
                        latestRevision = true,
                        percent = 100
                    }
                }
            }
        };

        // Use CreateNamespacedCustomObjectAsync to create the Knative service
        await _client.CreateNamespacedCustomObjectAsync(
            knativeService,
            "serving.knative.dev",   // Group
            "v1",                    // Version
            "pizza-app",             // Namespace
            "services",              // Resource (Kind)
            null);                   // Optionally, specify other parameters like `body`, if needed
    }
}
}

