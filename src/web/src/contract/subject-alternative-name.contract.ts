export interface SubjectAlternativeName {
  /** The kind of SAN */
  kind: "Dns" | "Email" | "IpAddress" | "UserPrincipal" | "Uri";

  /**
   * The subject alternative name. This has to match the kind. Example: kind = "Dns", name = "www.google.com"
   * */
  name: string;
}
