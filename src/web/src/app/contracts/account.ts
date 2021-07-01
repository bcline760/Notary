import { Entity } from "./entity";

export interface Account extends Entity {
    accountAddress?: AccountAddress;
    /**
     * The primary e-mail addressed used for this account.
     */
    email: string;

    /**
     * Account holder's first name
     */
    firstName: string;

    /**
     * Account holder's last name
     */
    lastName: string;

    /**
     * The role assigned to the account
     */
    roles: number;

    /**
     * The username used to login if the system is configured for it
     */
    username: string;
}

export interface AccountAddress {
    /** Get or set the primary address line which is the address and street name */
    addressLine?: string;

    /** Get or set the city of residence */
    city?: string;

    /** Get or set the country of residence */
    country?: string;

    /** Get or set the postal code*/
    postalCode?: string;

    /** Get or set the second address line which is usually the unit number */
    secondAddressLine?: string;

    /** Get or set the state or province within the country of residence */
    stateProvince?: string;

    /** Get or set the third address line which is usually an "attention" line. */
    thirdAddressLine?: string;
}
