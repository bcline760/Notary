export class Signin {
    private _username: string;
    private _password: string;
    private _persist: boolean;

    constructor(password?: string, persist?: boolean, username?: string) {
        this._password = password ? password : '';
        this._username = username ? username : '';
        this._persist = persist ? persist : false;
    }

    get password() { return this._password }
    set password(value: string) { this._password = value; }

    get persist() { return this._persist; }
    set persist(value: boolean) { this._persist = value; }

    get username() { return this._username }
    set username(value: string) { this._username = value; }
}
