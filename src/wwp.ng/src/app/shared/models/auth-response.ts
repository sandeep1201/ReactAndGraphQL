import { User } from './user';

export class AuthResponse {
  message: string;
  token: string;
  user: User;
}
