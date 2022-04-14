import { Role } from "../shared/role.model";

export class AppUser {
    name: string = '';
    email: string = '';
    roles: Role[] = [];
    isAuthenticated: boolean = false;
}
