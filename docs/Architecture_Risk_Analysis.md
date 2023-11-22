# Architecture Risk Analysis
Some specific analyses that can be done are:
- Boundary crossing
- Misuse cases
- Roles vs credentials
- How to secure Frontend & backend

## Application overview
Attached below is a container diagram of ICAP that serves as a quick overview of the different components that make up the project.

![C2 Diagram](Media/C2_Diagram.png)

ICAP is a marketing, forum and messenging platform in one. Its purpose is to serve as the international communications backbone for the airsoft community. 

## OWASP Top 10
The OWASP Top 10 Web Application Security Risks for 2021 highlights the most critical security risks to web applications. Here's a summary of each category:

1. **Broken Access Control**
   - Issues with applications failing to restrict access properly, allowing attackers to exploit permissions.

2. **Cryptographic Failures**
   - Focus on failures in cryptography, leading to data breaches. Previously known as Sensitive Data Exposure.

3. **Injection**
   - Various forms of injection attacks, like SQL injection, where malicious code is inserted into applications.

4. **Insecure Design**
   - A new category emphasizing the importance of secure design patterns and principles in application development.

5. **Security Misconfiguration**
   - Misconfigured permissions, unnecessary features, or verbose error messages exposing systems to attack.

6. **Vulnerable and Outdated Components**
   - Using components with known vulnerabilities, which can be a gateway for attacks.

7. **Identification and Authentication Failures**
   - Failures in correctly identifying and authenticating users, leading to unauthorized access.

8. **Software and Data Integrity Failures**
   - A new category addressing risks in software updates and data integrity, especially in CI/CD pipelines.

9. **Security Logging and Monitoring Failures**
   - Insufficient logging and monitoring, hindering breach detection and response.

10. **Server-Side Request Forgery (SSRF)**
   - Manipulating the server to send forged requests, potentially leading to unauthorized actions or data access.

Of these 10 risk categories, the ones that are the most interesting are:
- Broken Access Control
- Insecure Design
- Security Misconfiguration
- Vulnerable and Outdated Components
- Software and Data Integrity Failures

## Analysis based on CIA
