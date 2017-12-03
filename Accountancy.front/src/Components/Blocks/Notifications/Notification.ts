export class Notification {
    id: number;
    message: string;
    type: NotificationType;
}

export enum NotificationType {
    info = 0,
    warning = 1,
    error = 2
}

export default Notification;