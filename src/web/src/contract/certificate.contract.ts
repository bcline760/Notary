import { DistinguishedName } from "./distinguished-name.contract";
import { Entity } from "./entity.contract";
import { SubjectAlternativeName } from "./subject-alternative-name.contract";

export interface Certificate extends Entity {
    /** The algorithm used to create the certificate */
    algorithm: "RSA" | "EllipticCurve";

    /** The distinguished name of the issuer */
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

    /** Get the certificate's subject DN */
    subject: DistinguishedName;

    /** SHA1 Thumbprint identifying the certificate */
    thumbprint: string;
}
