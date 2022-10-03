export class UserDTO {
    name: string = "";
    position: string = "";
    email: string = "";
    avatar: string = "";

    constructor(name: string, position: string, email: string, avatar: string){
        this.name = name;
        this.position = position;
        this.email = email;
        this.avatar = avatar;
    }
}
