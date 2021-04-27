export interface DistinguishedName {
  /** The "Common Name" portion of the DN */
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
