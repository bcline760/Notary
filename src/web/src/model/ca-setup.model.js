"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.CaSetup = void 0;
var CaSetup = /** @class */ (function () {
    function CaSetup() {
        this._rootCn = "";
        this._intermediateCn = "";
        this._keyLength = 0;
        this._expiryYears = 0;
    }
    Object.defineProperty(CaSetup.prototype, "rootCommonName", {
        /**
        * The common name for the root certificate
        */
        get: function () { return this._rootCn; },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(CaSetup.prototype, "intermediateCommonName", {
        /**
         * The common name for the intermediate signing certificate
         */
        get: function () { return this._intermediateCn; },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(CaSetup.prototype, "organization", {
        /**
         * Get the organization of the CA
         */
        get: function () { return this._organization; },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(CaSetup.prototype, "organizationalUnit", {
        /**
        * The organizational unit of the CA
        */
        get: function () { return this._orgUnit; },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(CaSetup.prototype, "locale", {
        /**
         * The locale (city, town) of the CA
         */
        get: function () { return this._locale; },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(CaSetup.prototype, "stateProvince", {
        /**
         * The state or province of the CA
         */
        get: function () { return this._state; },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(CaSetup.prototype, "country", {
        /**
         * The country in which the CA resides
         */
        get: function () { return this._country; },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(CaSetup.prototype, "keyLength", {
        /**
         * The length of the private key
         */
        get: function () { return this._keyLength; },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(CaSetup.prototype, "expiryInYears", {
        /**
         * The number of years the certificate is valid
         */
        get: function () { return this._expiryYears; },
        enumerable: false,
        configurable: true
    });
    return CaSetup;
}());
exports.CaSetup = CaSetup;
//# sourceMappingURL=ca-setup.model.js.map