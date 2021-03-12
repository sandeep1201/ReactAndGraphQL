
// Code based upon http://stackoverflow.com/questions/37144672/guid-uuid-type-in-typescript

export class Guid {

  private str: string;

  private static newGuidString(): string {
    // Your favourite guid generation function could go here
    // ex: http://stackoverflow.com/a/8809472/188246
    let d = new Date().getTime();
    if (window.performance && typeof window.performance.now === "function") {
      d += performance.now(); // Use high-precision timer if available
    }
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, (c) => {
      let r = (d + Math.random() * 16) % 16 | 0;
      d = Math.floor(d / 16);
      return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
    });
  }

  constructor(str?: string) {
    this.str = str || Guid.newGuidString();
  }

  toString() {
    return this.str;
  }
}
