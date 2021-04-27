export class CaSetup {
  private _rootCn: string;
  private _intermediateCn: string;
  private _organization: string | null;
  private _orgUnit: string | null;
  private _locale: string | null;
  private _state: string | null;
  private _country: string | null;
  private _keyLength: number;
  private _expiryYears: number;

  constructor() {
    this._rootCn = "";
    this._intermediateCn = "";
    this._keyLength = 0;
    this._expiryYears = 0;
  }

  /**
  * The common name for the root certificate
  */
  get rootCommonName() { return this._rootCn; }
  /**
   * The common name for the intermediate signing certificate
   */
  get intermediateCommonName() { return this._intermediateCn; }
  /**
   * Get the organization of the CA
   */
  get organization() { return this._organization; }
  /**
  * The organizational unit of the CA
  */
  get organizationalUnit() { return this._orgUnit; }
  /**
   * The locale (city, town) of the CA
   */
  get locale() { return this._locale; }
  /**
   * The state or province of the CA
   */
  get stateProvince() { return this._state; }
  /**
   * The country in which the CA resides
   */
  get country() { return this._country; }

  /**
   * The length of the private key
   */
  get keyLength() { return this._keyLength; }

  /**
   * The number of years the certificate is valid
   */
  get expiryInYears() { return this._expiryYears; }
}
