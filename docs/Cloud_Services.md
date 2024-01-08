# Cloud Services Overview and Cost Considerations
This document offers a detailed overview of various cloud services being utilized within the ICAP project, highlighting their specific roles and the cost considerations associated with each. The selection ranges from MongoDB in its serverless tier, suitable for flexible testing and educational scenarios, to a comprehensive array of Azure-related cloud services that address diverse needs in application development and deployment.

Key Azure services such as Azure Entra Identity, Azure Kubernetes Service (AKS), Azure Key Vault, Azure Static Web App, Azure Container Registry, and Azure Service Bus are elaborated upon, underscoring their importance in aspects like authentication, container orchestration, secret management, web hosting, image repository, and inter-service communication. By understanding their pricing models and functionalities, the ICAP project can effectively manage resources and budgeting, especially during transitions from development/testing phases to production environments.

## MongoDB Serverless Tier
- **Usage**: Ideal for testing workloads and educational purposes due to its flexibility and scalability.
- **In Production**: For a production environment, a dedicated version is recommended for enhanced performance and control.
- **Cost Consideration**: The serverless tier charges $0.1 per million reads. Offers pay-per-operation pricing, meaning you pay for the Processing Units consumed by your database operations and storage consumed by your data and indexes.

## Azure Cloud Services

### Azure Entra Identity (Formerly Azure AD)
- **Usage**: Provides comprehensive solutions for authentication, authorization, and user/application management. Integrates seamlessly with Azure Role-Based Access Control (RBAC).
- **Cost Consideration**: Azure AD External Identities pricing is based on Monthly Active Users (MAU). The first 50,000 MAUs per month are free for both Premium P1 and Premium P2 features.

### Azure Kubernetes Service (AKS)
- **Usage**: Serves as the primary Kubernetes managed environment for application backends in a dev/test setting.
- **In Production**: Switching to the AKS Enterprise option is advisable for better integration with Azure Entra for auth management and availability of the Secret Store CSI driver.
- **Cost Consideration**: AKS is a free container service, charging only for the nodes it uses to build containers and other cloud resources like VMs, storage, and networks. The Standard tier costs $0.10 per cluster per hour and the Premium tier at $0.60 per cluster per hour.

### Azure Key Vault
- **Usage**: Utilized for secure storage and management of secrets, keys, and certificates. Integrates with Azure services for seamless access management.
- **Cost Consideration**: Secrets operations cost $0.03/10,000 transactions, Certificate operations cost $3 per renewal request and $0.03/10,000 transactions for all other operations. Managed HSM Pools have varying costs, e.g., Standard B1 at $3.20 Hourly usage fee per HSM pool.

### Azure Static Web App
- **Usage**: Hosts the client-side of the system, providing a scalable and secure web hosting service.
- **In Production**: Switching to the Standard tier for general purpose production apps is recommended.
- **Cost Consideration**: Offers both a Free and Standard tier, with the Standard Tier costing ~$9/per app/month. If he Enterprise-grade edge solution is used then an additional $17.52/per app/month is charged.


### Azure Container Registry (ACR)
- **Usage**: Acts as a repository for container images, integrating with CI/CD pipelines for image storage and retrieval.
- **Cost Consideration**: Azure Container Registry pricing has 3 different tiers to it, between which the included storage is the most significant difference (10GB for Basic, 100 for Standard and 500 for Premium). The prices are a fixed amount per day; being $0.167 for basic, $0.667 for Standard and $1.667 for Premium. Premium has 2 extra benefits which could also be very important for large scale cloud operations, which are the possibility to use Geo Replication for the ACR and the enhanced throughput for docker pulls across multiple, concurrent nodes.

### Azure Service Bus
- **Usage**: Facilitates pub/sub messaging and other patterns for inter-service communication, ensuring reliable message delivery.
- **Cost Consideration**: To be able to use pub/sub messaging using what Azure calls "Topics", you'll need at least the standard tier of the service bus. The pricing for the standard tier looks like this one below.


| Description                             | Cost                         |
|-----------------------------------------|------------------------------|
| Base charge                             | $0.0135/hour                 |
| First 13M operations/month               | Included                     |
| Next 87M ops (13-100M ops)/month         | $0.80 per million operations |
| Next 2,400M ops (100-2,500M ops)/month   | $0.50 per million operations |
| Over 2,500M ops/month                    | $0.20 per million operations |

For the premium version, the pricing is very simple at a fixed cost of $0.928/hour.

### Sonarcloud 
- **Usage**: Sonarcloud is used in the CI/CD pipeline of ICAP to perform static code analyses. Using these static code analyses, vulnerabilities and flaws within ICAP can be easily identified and solved.

## Sources
- https://www.mongodb.com/atlas/serverless
- https://azure.microsoft.com/en-us/pricing/details/active-directory/external-identities/
- https://azure.microsoft.com/en-us/pricing/details/kubernetes-service/
- https://www.saasworthy.com/product/azure-key-vault/pricing
- https://azure.microsoft.com/en-us/pricing/details/app-service/
- https://azure.microsoft.com/en-us/pricing/details/container-registry/
- https://azure.microsoft.com/en-us/pricing/details/service-bus/
