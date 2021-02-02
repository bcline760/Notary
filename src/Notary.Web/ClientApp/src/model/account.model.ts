export interface Account {
    accountAddress: any;
    email: string;
    firstName: string | null;
    lastName: string | null;
    publicKey: string;
    roles: number;
    username: string;
}

export interface Address {
    addressLine: string | null;
    city: string | null;
    country: string | null;
    postalCode: string | null;
    secondAddressLine: string | null;
    stateProvince: string | null;
    thirdAddressLine: string | null;
}
