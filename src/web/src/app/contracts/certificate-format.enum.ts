export enum CertificateFormat {
    /**
     * Format the certificate in DER
     */
    Der = 'Der',
    /**
     * Format the certificate in PKCS #7
     */
    PKCS7 = 'Pkcs7',
    /**
     * Format the certificate in PKCS #12 which can include the private key
     */
    PKCS12 = 'Pkcs12',

    /**
     * Format certificate in PEM
     */
    PEM = 'Pem'
}