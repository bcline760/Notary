import { Data } from "./data.model";

/** Models a certificate data object which defines a X.509 certificate */
export interface Certificate extends Data {
    /**
     *
     */
    algorithm: string;

    /**
    * The certificate issuer's X.500 distinguished name
    */
    issuer: DistinguishedName | null;

    /**
    *
    */
    keyUsage: number;

    /**
    * Get whether this certificate is used to issue other certificates
    */
    primarySigningCertificate: boolean;

    /**
    * Certificate is valid before the given date
    */
    notBefore: Date;

    /**
    * Certificate is valid after the given date
    */
    notAfter: Date;

    /**
    * Date the certificate was revoked
    */
    revocationDate: Date | null;

    /**
    * The serial number of the certificate
    */
    serialNumber: string;

    /**
    * The slug of the certificate used to sign this certificate
    */
    signingCertificateSlug: string;

    /**
    * The certificate subject as a X.500 distinguished name
    */
    subject: DistinguishedName | null;

    /**
    * This of subject alternative names in the certificate.
    */
    subjectAlternativeNames: SubjectAlternativeName[] | null;

    /**
    * The unique thumbprint of the certificate
    */
    thumbprint: string;
}

/**
 * Represents a X.500 distinguished name */
export interface DistinguishedName {
    /**
    * The common name of the certificate (CN)
    */
    commonName: string | null;

    /**
    * The country of origin of the certificate (C)
    */
    country: string | null;

    /**
    * The locale (city) of origin (L)
    */
    locale: string | null;

    /**
    * The organization name. Like a company, or a non-profit, (O)
    */
    organization: string | null;

    /**
    * The organizational unit (like department) within the organization (OU)
    */
    organizationalUnit: string | null;

    /**
    * The state or province of orgin (ST)
    */
    stateProvince: string | null;
}

export interface SubjectAlternativeName {

}
