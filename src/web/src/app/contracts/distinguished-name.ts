export interface DistinguishedName {
    /** The "Common Name" portion of the DN */
    commonName: string;

    /** The "Country" portion of the DN */
    country: string | null;

    /** The "Locale" portion of the DN" */
    locale: string | null;

    /** the "Organization" part of the DN */
    organization: string | null;

    /** The "Organizational Unit" part of the DN */
    organizationalUnit: string | null;

    /** The "State/Province" of the DN */
    stateProvince: string | null;
}