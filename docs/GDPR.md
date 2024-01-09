# GDPR Checklist

([source](https://gdpr.eu/checklist/))

## Lawful basis and transparency

<details><summary>Conduct an information audit to determine what information you process and who has access to it.</summary>

**Findings**

As this project is brand-new, there is no previous data that has to be further audited. All of the data that will be collected and processed can be inferred from the documentation and the architecture design.

**Advice**

Write down what information is collected, where it is stored, and who has access to it when the application goes live.

</details>

<details><summary>Have a legal justification for your data processing activities.</summary>

**Findings**

Data processed is solely used for the application's core functionality. None of the data is processed for other reasons.

**Advice**

Not applicable.

</details>

<details><summary>Provide clear information about your data processing and legal justification in your privacy policy.</summary>

**Findings**

There is no privacy policy for ICAP yet.

**Advice**

A privacy policy should be created and integrated into the application. This privacy policy should be easy to find and should be displayed to the user before any data is processed.

</details>

## Data security

<details><summary>Take data protection into account at all times, from the moment you begin developing a product to each time you process data.</summary>

**Findings**

Data protection was taken into account from the start of the project.

**Advice**

If there is new data to be processed within the application, the risk analysis should be revisited to assess what type of data it falls under and what the use of this data is within the application context. If needs be, the privacy policy can then be updated accordingly and an email should be sent to all existing users about the privacy policy updates.

</details>

<details><summary>Encrypt, pseudonymize, or anonymize personal data wherever possible.</summary>

**Findings**

The personal data used within the application is the bare-minimum needed to make the application functional. The only thing that can be done to further protect this data is to restrict access to the database in which the data is saved. It can not be further anonymized or pseudonymized.

**Advice**

Give users the option to hide their address details and make it so users are known by their nickname and can be given the option to show their full name to friends.

</details>

<details><summary>Create an internal security policy for your team members, and build awareness about data protection.</summary>

**Findings**

There is no internal security policy needed for ICAP. ICAP is created by a single developer that regularly reads about data protection, because of the needs for achieving their learning outcomes and to satisfy their interests and concerns about privacy.

**Advice**

When the project is expanded beyond a single developer, a security policy should then be created.

</details>

<details><summary>Know when to conduct a data protection impact assessment, and have a process in place to carry it out.</summary>

**Findings**

ICAP's developer currently does not know when to conduct a data protection impact assessment and there is no process in place to carry it out.

**Advice**

Learn when to conduct a data protection impact assessment and create a process to carry it out.

</details>

<details><summary>Have a process in place to notify the authorities and your data subjects in the event of a data breach.</summary>

**Findings**

There currently is no procedure in place to notify the authorities and/or customers of a data breach.

**Advice**

Set up a procedure to notify the authorities and/or customers in the event of a data breach.

</details>

## Accountability and governance

<details><summary>Designate someone responsible for ensuring GDPR compliance across your organization.</summary>

**Findings**

Due to ICAP being developed by one person, there is no separate person responsible for GDPR compliance. Only the developer, is responsible for GDPR compliance.

**Advice**

Appoint a person to be responsible for GDPR compliance when the project grows.

</details>

<details><summary>Sign a data processing agreement between your organization and any third parties that process personal data on your behalf.</summary>

**Findings**

ICAP does not use any third parties to process personal data and does not plan on doing so in the foreseeable future. There is a possibility that the existing technologies used within ICAP have service agreements concerning data processing.

**Advice**

Carefully read the terms of service when using third-party services or technologies that touch the application's data.

</details>

<details><summary>If your organization is outside the EU, appoint a representative within one of the EU member states.</summary>

**Findings**

ICAP operates wholly from within the EU.

**Advice**

No external, EU-based representative is necessary.

</details>

<details><summary>Appoint a Data Protection Officer (if necessary)</summary>

**Findings**

ICAP is not in whole or part of a public authority. It also does not conduct large-scale operations concerning GDPR data. If the project grows beyond that threshold, a DPO may be necessary.

**Advice**

No DPO is needed if the company does not process data on a large scale or in such a way that it requires a DPO. ([source](https://gdpr.eu/data-protection-officer/#:~:text=All%20organizations%2C%20regardless,of%20three%20criteria%3A)).

</details>

## Privacy rights

<details><summary>It's easy for your customers to request and receive all the information you have about them.</summary>

**Findings**

There is currently no system in place for customers to request and receive all the information ICAP has about them.

**Advice**

Design and implement a mechanism that allows a user's data to be requested.

</details>

<details><summary>It's easy for your customers to correct or update inaccurate or incomplete information.</summary>

**Findings**

The application allows customers to update their own information. To correct information displayed by other users or the application itself, there is no such system.

**Advice**

Consider implementing a suggestions or anonymized feedback section to be able to correct inaccurate or incomplete information. A disclaimer should be added to the ToS about misinformation given spread by other users.

</details>

<details><summary>It's easy for your customers to request to have their personal data deleted.</summary>

**Findings**

The application allows for data deletion, but it has not been implemented yet.

**Advice**

Implement data deletion.

</details>

<details><summary>It's easy for your customers to ask you to stop processing their data.</summary>

**Findings**

ICAP does not process data for any other reason besides the required data for the application to function correctly. If the customer would like their data to not be processed anymore, they can request their account to be deleted.

**Advice**

Not applicable.

</details>

<details><summary>It's easy for your customers to receive a copy of their personal data in a format that can be easily transferred to another company.</summary>

**Findings**

Currently, data take-out is not yet implemented. When it is, data will be presented in a human-readable format that can also be processed by a computer, such as JSON.

**Advice**

Implement data take-out.

</details>

<details><summary>It's easy for your customers to object to you processing their data.</summary>

**Findings**

As ICAP only processes data that is required for the essential functionality of the application, users can only object to their data being processed by not signing up or by removing their account.

**Advice**

Not applicable.

</details>

<details><summary>If you make decisions about people based on automated processes, you have a procedure to protect their rights.</summary>

**Findings**

There are no automated decisions about people.

**Advice**

Not applicable.

</details>

## Summary of advice:

Below here is a list that summarizes all the action points needed to make ICAP fully GDPR compliant before release.

- Create a ledger for where information is stored, and who has access to it.
- Write a privacy policy.
- Keep data protection in mind at all times & update risk analysis and privacy policy if needed.
- Add the option to hide address information, make users primarily known to each other by their nicknames and have an option to show full name to friends.
- Create an internal security policy.
- Learn when to conduct a data protection impact assessment and create a process to carry it out.
- Set up a procedure to notify the authorities and/or customers in the event of a data breach.
- Carefully read the terms of service when using third-party services or technologies that touch ICAP's data.
- Implement a suggestions or anonymized feedback section to be able to correct inaccurate or incomplete information or add a disclaimer about misinformation.
- Design and implement a mechanism that allows data take-out.
- Implement data deletion.