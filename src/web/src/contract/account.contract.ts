import { Entity } from "./entity.contract";

export interface Account extends Entity {
    accountAddress?: AccountAddress;
    email: string;
    firstName: string;
    lastName: string;
    publicKey?: string;
    roles: number;
    username: string;
}

/** Contract that describes a building address */
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
