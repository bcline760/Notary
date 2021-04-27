import { DistinguishedName } from "./distinguished-name.contract";

export interface CertificateAuthoritySetup {
  /**
  * The root distinguished name
  * */
  rootDn: DistinguishedName;

  /**
  * The intermediate signing certificate distinguished name
  * */
  intermediateDn: DistinguishedName;

  /**
  * The length of the RSA key, standard is 2048
  * */
  keyLength: number;

  /**
  * The number of years until the CA certificates expire. Industry standard is 10 years
  * */
  lengthInYears: number;

  /**
   * Who created the certificate authority.
   * */
  requestor: number;
}
