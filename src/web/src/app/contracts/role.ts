export enum Role {
    /**
     * This role has full access to all of Notary
     */
    Admin = 0,
    /**
     * This role can issue, revoke, and delete certificates
     */
    CertificateAdmin = 1,
    /**
     * This is a basic role. Can only view and download certificates
     */
    User = 2
}
