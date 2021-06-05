/**
 * The kinds of SAN entries that can be used in a X.509 certificate
 */
export enum SanKind {
    /**
     * A DNS subject alternative name
     */
    dns = 0,

    /**
     * An e-mail as a subject alternative name
     */
    email = 1,

    /**
     * A specific IP address as a SAN
     */
    ipAddress = 2,

    /**
     * A subject alternative name which is a user principal like a username
     */
    userPrincipal = 3,

    /**
     * A uniform resource indicator SAN entry
     */
    uri = 4
}