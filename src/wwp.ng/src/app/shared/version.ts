// http://semver.org
// tslint:disable: no-shadowed-variable
export class SemanticVersion {
  private _build: string;
  public get build(): string {
    return this._build;
  }
  public set build(val: string) {
    this._build = val != null ? val.toString() : null;
  }
  constructor();
  constructor(major: number, minor: number, patch: number, build?: string);
  constructor(public major: number = null, public minor: number = null, public patch: number = null, build: string = null) {
    this.build = build;
  }

  public static equals(first: SemanticVersion, second: SemanticVersion) {
    if ((second == null && first != null) || (second != null && first == null)) {
      return false;
    } else if (first == null && second == null) {
      return true;
    }

    return first.major === second.major && first.minor === second.minor && first.patch === second.patch && first.build === second.build;
  }

  public static parse(versionString: string) {
    if (!versionString) {
      return null;
    }

    const parts = versionString.split('.');
    if (parts.length < 3) {
      return null;
    }
    const obj: SemanticVersion = new SemanticVersion();
    obj.major = +parts[0];
    obj.minor = +parts[1];
    obj.patch = +parts[2];
    obj.build = parts.length > 3 ? parts[3] : null;
    return obj;
  }

  public equals(other: SemanticVersion) {
    return SemanticVersion.equals(this, other);
  }

  toString() {
    let sem = `${this.major}.${this.minor}.${this.patch}`;
    if (this.build != null) {
      sem = sem + `.${this.build}`;
    }
    return sem;
  }

  private parse(versionString: string): boolean {
    if (!versionString) {
      return false;
    }

    const parts = versionString.split('.');
    if (parts.length < 3) {
      return false;
    }
    this.major = +parts[0];
    this.minor = +parts[1];
    this.patch = +parts[2];
    this.build = parts.length > 3 ? parts[3] : null;
    return true;
  }
}

export const version = new SemanticVersion(2, 2, 2, '25');

export const build = '';
