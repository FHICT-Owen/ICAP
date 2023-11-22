# Architecture Risk Analysis
This document serves to analyse the security vulnerabilities within ICAP. After a brief overview of the application, various security analyses will be applied in order to find out how ICAP should be made secure by design.

## Application Overview
Attached below is a container diagram of ICAP that serves as a quick overview of the different components that make up the project. ICAP is a marketing, forum and messenging platform in one. Its purpose is to serve as the international communications backbone for the airsoft community. In order to be able to scale reliably for the hundreds of thousands of possible users within the international airsoft community, a microservices architecture was chosen for the project.

![C2 Diagram](Media/C2_Diagram.png)

The microservices architecture allows for improved scaling, fault isolation and service resilliency among a few other things. Because of the highly decoupled nature of microservices it also means that there are possibly more vulnerabilities in the system due to communication between services having to take place over the web instead of within the server. Furthermore, requests from the front-end also have to be routed through the individual microservices

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

All of the OWASP top 10 security risks are very important to take into account, but in order to make ICAP more secure the security risks above should be applied in a more practical way. In order to achieve that, it is important to create misuse cases that apply specifically to ICAP and figure out how to mitigate or solve those vulnerabilities alltogether. This will be addressed in the next section.

## Examplary Misuse Cases Based On OWASP
Below are some examplary misuse cases where a bad actor tries to gain access to the system. These examples are ordered in the same way as the top 10 in the section above. These cases serve as generic misuse cases that can then be applied to ICAP in order to get applicable misuse cases and solve possible security vulnerabilities.

1. An attacker exploits access control mechanisms to gain unauthorized access to data or functionality, modify data, or perform actions that should be restricted to the user type he has access to.
2. An attacker takes advantage of weak or improperly implemented cryptography to access sensitive data, such as intercepting data in transit or decrypting sensitive data stored insecurely.
3. An attacker sends malicious data to the system, leading to the execution of unintended commands or accessing unauthorized data, for example, SQL injection, command injection, or LDAP injection.
4. An attacker exploits design flaws in the application, potentially leading to widespread security issues, due to lack of proper threat modeling or secure design patterns.
5. An attacker takes advantage of insecure default configurations, incomplete setups, open cloud storage, misconfigured HTTP headers, or verbose error messages to gain unauthorized access or knowledge of the system.
6. An attacker targets known vulnerabilities in outdated or unpatched components (libraries, frameworks, software modules) to compromise the system or steal data.
7. An attacker exploits weaknesses in authentication and identification mechanisms to impersonate legitimate users, such as through credential stuffing, brute force attacks, or exploiting improperly implemented multi-factor authentication.
8. An attacker manipulates the integrity of software or data, for example, through unauthorized code changes, tampering with data, or Man-in-the-Middle attacks against software update mechanisms.
9. Due to inadequate logging and monitoring, an attacker’s malicious activities go undetected, allowing them to maintain persistence, escalate privileges, or perform additional attacks without being noticed.
10. An attacker manipulates the server to perform unintended requests to internal systems, potentially leading to information disclosure, internal system enumeration, or other malicious activities​​.

## Analysis Based On CIAP
CIAP is a metric used to classify data into a scale that is easy to understand. It consists of the following classifications:
- **Availability**: not needed (0), important (1), required (2), essential (3)
- **Integrity**: unprotected (0), protected (1), high (2), absolute (3)
- **Confidentiality**: public (0), company confidential (1), confidential (2), classified (3)
- **Privacy**: public (0), PII (p)

### Cases To Analyze with CIAP