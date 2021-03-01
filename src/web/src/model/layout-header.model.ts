import { Role } from "src/contract/role.enum";

export class LayoutHeader {
    private _accountSlug: string;
    private _fullName: string;
    private _role: Role;

    constructor() { }

    /**
     * Get the user's application database ID
     */
    public get accountSlug(): string { return this._accountSlug; }
    /**
     * Set the user's application database ID in the model
     */
    public set accountSlug(value: string) { this._accountSlug = value; }

    /**
     * Get the full name of the current user
     */
    public get fullName(): string { return this._fullName; }
    /**
     * Set the full name of the current user
     */
    public set fullName(value: string) { this._fullName = value; }

    /**
     * Get the user's role in the model
     */
    public get role(): Role { return this._role; }
    /**
     * Set the user's role in the model
     */
    public set role(value:Role){this._role=value;}
}
