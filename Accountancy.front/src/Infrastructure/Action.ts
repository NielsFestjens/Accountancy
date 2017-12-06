export default interface Action<T = any>{
  type: string;
  payload?: T;
  error?: boolean;
  meta?: any;
}