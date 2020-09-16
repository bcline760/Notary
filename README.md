# Notary

My attempt at a .NET based PKI.

# Overview

Well I built a little mini infrastructure of Raspberry Pi servers cobbled together by a Domain Controller. I want to secure them with TLS/SSL but, I don't want to self-sign everything and have a billion root certs. I tried my hand at AD CS and it's absolute garbage. Provisioning certificates using AD CS is stuck in 2003 and has virtually no API. I couldn't find a PKI solution for people like me who don't have the coin to pony up for something like Venafi. Let's Encrypt doesn't work for me because I don't have a domain (yet). So, I thought for shits and giggles, I'd give it ago at making a PKI.