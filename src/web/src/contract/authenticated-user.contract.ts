import { Entity } from "./entity.contract";
import { Role } from "./role.enum";

export interface AuthenticatedUser extends Entity {
    /** The slug belonging to the account*/
    accountSlug: string;
    expiry: Date
    fName?: string;
    lName?: string;
    login?: string;
    role: Role;
    token: string;
}
