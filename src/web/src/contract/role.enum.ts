export enum Role {
    /**
     * This role has full access to all of Notary
     */
    Admin = 'Admin',
    /**
     * This role can issue, revoke, and delete certificates
     */
    CertificateAdmin = 'CertificateAdmin',
    /**
     * This is a basic role. Can only view and download certificates
     */
    User = 'User'
}
