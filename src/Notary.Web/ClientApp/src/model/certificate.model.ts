import { Data } from "./data.model";

/** Models a certificate data object which defines a X.509 certificate */
export class Certificate extends Data {

    /** Construct a new Certificate data object */
    constructor() {
        super();

        this.active = false;
        this.algorithm = '';
        this.issuer = null;
        this.keyUsage = 0;
        this.primarySigningCertificate = false;
        this.notAfter = new Date();
        this.notBefore = new Date();
        this.revocationDate = null;
        this.serialNumber = '';
        this.signingCertificateSlug = '';
        this.subject = null;
        this.subjectAlternativeNames = null;
        this.thumbprint = '';
    }

    /**
     *
     */
    public algorithm: string;

    /**
    * The certificate issuer's X.500 distinguished name
    */
    public issuer: DistinguishedName | null;

    /**
    *
    */
    public keyUsage: number;

    /**
    * Get whether this certificate is used to issue other certificates
    */
    public primarySigningCertificate: boolean;

    /**
    * Certificate is valid before the given date
    */
    public notBefore: Date;

    /**
    * Certificate is valid after the given date
    */
    public notAfter: Date;

    /**
    * Date the certificate was revoked
    */
    public revocationDate: Date | null;

    /**
    * The serial number of the certificate
    */
    public serialNumber: string;

    /**
    * The slug of the certificate used to sign this certificate
    */
    public signingCertificateSlug: string;

    /**
    * The certificate subject as a X.500 distinguished name
    */
    public subject: DistinguishedName | null;

    /**
    * This of subject alternative names in the certificate.
    */
    public subjectAlternativeNames: SubjectAlternativeName[] | null;

    /**
    * The unique thumbprint of the certificate
    */
    public thumbprint: string;
}

/**
 * Represents a X.500 distinguished name */
class DistinguishedName {
    constructor() {
        this.commonName = null;
        this.country = null;
        this.locale = null;
        this.organization = null;
        this.organizationalUnit = null;
        this.stateProvince = null;
    }

    /**
    * The common name of the certificate (CN)
    */
    public commonName: string | null;

    /**
    * The country of origin of the certificate (C)
    */
    public country: string | null;

    /**
    * The locale (city) of origin (L)
    */
    public locale: string | null;

    /**
    * The organization name. Like a company, or a non-profit, (O)
    */
    public organization: string | null;

    /**
    * The organizational unit (like department) within the organization (OU)
    */
    public organizationalUnit: string | null;

    /**
    * The state or province of orgin (ST)
    */
    public stateProvince: string | null;
}

class SubjectAlternativeName {

}
