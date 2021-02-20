export class LoginRequest {
    private _username: string;
    private _password: string;
    private _persist: boolean;
    constructor() {
        this._password = '';
        this._persist = false;
        this.username = '';
    }

    public get username(): string {
        return this._username;
    }
    public set username(value: string) {
        this._username = value;
    }

    public get password(): string {
        return this._password;
    }
    public set password(value: string) {
        this._password = value;
    }

    public get persist(): boolean {
        return this._persist;
    }
    public set persist(value: boolean) {
        this._persist = value;
    }
}