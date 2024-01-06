# Cloud Services Overview and Cost Considerations

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
- **Usage**: Facilitates pub/sub messaging patterns for inter-service communication, ensuring reliable message delivery.
- **Cost Consideration**: Typically, the cost is based on the number of messages and the size of the messages, with different pricing tiers offering varying levels of throughput and features. (Cost details are not available yet).

## Sources
- https://www.mongodb.com/atlas/serverless
- https://azure.microsoft.com/en-us/pricing/details/active-directory/external-identities/
- https://azure.microsoft.com/en-us/pricing/details/kubernetes-service/
- https://www.saasworthy.com/product/azure-key-vault/pricing
- https://azure.microsoft.com/en-us/pricing/details/app-service/
- https://azure.microsoft.com/en-us/pricing/details/container-registry/
