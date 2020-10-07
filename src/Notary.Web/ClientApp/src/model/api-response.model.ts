export class ApiResponse<T> {
  constructor() {
    this.data = null;
    this.message = null;
    this.success = false;
  }

  /**
   * Data returned back from the server
   * 
   * @typeParam T The type of data expected back from the server
   */ 
  public data: T | null;

  /**
   * Message returned back from the server
   */
  public message: string | null;

  /**
   * Indicates whether the resulting API call is a success
   */
  public success: boolean;
}
