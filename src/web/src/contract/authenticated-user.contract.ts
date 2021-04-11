import { Entity } from "./entity.contract";
import { Role } from "./role.enum";

export interface AuthenticatedUser extends Entity {
    /** The slug belonging to the account*/
    accountSlug: string;
    /** The expire time of the authenticated user */
    expiry: Date

    /** The first name of the authenticated user */
    firstName?: string;

    /** The last name of the authenticated user */
    lastName?: string;

    /** The user's login*/
    login?: string;

    /** The user's role */
    role: Role;

    /** The JWT generated from the API */
    token: string;
}
