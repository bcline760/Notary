"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.AuthGuardService = void 0;
var AuthGuardService = /** @class */ (function () {
    function AuthGuardService(tokenService, router) {
        this.tokenService = tokenService;
        this.router = router;
    }
    AuthGuardService.prototype.canActivate = function (route, state) {
        return !this.tokenService.checkExpired();
    };
    return AuthGuardService;
}());
exports.AuthGuardService = AuthGuardService;
//# sourceMappingURL=auth-guard.service.js.map