import { Entity } from "./entity.contract";

export interface Certificate extends Entity {
    algorithm: "RSA" | "EllipticCurve";

    issuer: DistinguishedName;

    keyUsage: number;

    /** Get or set whether this certificate can sign other certificates */
    isPrimarySigning: boolean;

    /** Certificate is valid not after this date */
    notAfter: Date;

    /** Certificate is valid not before this date */
    notBefore: Date;

    /** Date which the certificate was revoked */
    revokeDate: Date | null;

    /** The certificate serial number */
    sn: string;

    /** The certificate slug used to sign the certificate. */
    signingCertSlug: string | null;

    /** List of subject alternative names attached to certificate */
    sanList: SubjectAlternativeName[] | null;

    /** SHA1 Thumbprint identifying the certificate */
    thumbprint: string;
}

export interface DistinguishedName {
    cn: string;

    /** The "Country" portion of the DN */
    c: string | null;

    /** The "Locale" portion of the DN" */
    l: string | null;

    /** the "Organization" part of the DN */
    o: string | null;

    /** The "Organizational Unit" part of the DN */
    ou: string | null;

    /** The "State/Province" of the DN */
    s: string | null;
}

export interface SubjectAlternativeName {
    /** The kind of SAN */
    kind: "Dns" | "Email" | "IpAddress" | "UserPrincipal" | "Uri";

    name: string;
}