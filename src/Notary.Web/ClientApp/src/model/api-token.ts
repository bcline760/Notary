import { Data } from "./data.model";

export interface ApiToken extends Data {
    /**
     * The account associated with the token
     * */
    accountSlug: string;

    /**
     * When the JWT expires
     * */
    expiry: Date;

    /**
     * The JWT returned
     * */
    token: string;
}
