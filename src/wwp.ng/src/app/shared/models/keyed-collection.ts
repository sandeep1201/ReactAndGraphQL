
export interface IKeyedCollection<T> {
  add(key: string, value: T);
  containsKey(key: string): boolean;
  count(): number;
  item(key: string): T;
  keys(): string[];
  remove(key: string): T;
  values(): T[];
}

export class Dictionary<T> implements IKeyedCollection<T> {
  private items: { [index: string]: T } = {};

  private _count = 0;

  public containsKey(key: string): boolean {
    return this.items.hasOwnProperty(key);
  }

  public count(): number {
    return this._count;
  }

  public add(key: string, value: T) {
    this.items[key] = value;
    this._count++;
  }

  public remove(key: string): T {
    const val = this.items[key];
    delete this.items[key];
    this._count--;
    return val;
  }

  public item(key: string): T {
    return this.items[key];
  }

  public keys(): string[] {
    return Object.keys(this.items);
  }

  public values(): T[] {
    const values: T[] = [];

    for (const prop in this.items) {
      if (this.items.hasOwnProperty(prop)) {
        values.push(this.items[prop]);
      }
    }

    return values;
  }
}
