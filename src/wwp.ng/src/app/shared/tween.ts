export class Tween {
	private static _tweens: Tween[] = [];

	static getAll() {
		return Tween._tweens;
	}

	static removeAll() {
		Tween._tweens.length = 0;
	}

	static add(tween: Tween) {
		Tween._tweens.push(tween);
	}

	static remove(tween: Tween) {
		const index = Tween._tweens.indexOf(tween);
		if (index !== -1) {
			Tween._tweens.splice(index, 1);
		}
	}

	static update(time?: number, preserve = false) {
		if (Tween._tweens.length === 0) {
			return false;
		}

		let i = 0;
		time = time != null ? time : Tween.Now();

		while (i < Tween._tweens.length) {

			if (Tween._tweens[i].update(time) || preserve) {
				i++;
			} else {
				Tween._tweens.splice(i, 1);
			}

		}
		return true;
	}

	static Now() {
		return performance.now();// TODO: Polyfille for IE9?
	}


	/* Start Instance members */
	private _object: any;
	private _valuesStart: any = {};
	private _valuesEnd: any = {};
	private _valuesStartRepeat: any = {};
	private _duration = 1000;
	private _repeat = 0;
	private _repeatDelayTime: number;
	private _yoyo = false;
	private _isPlaying = false;
	private _reversed = false;
	private _delayTime = 0;
	private _startTime = null;
	private _easingFunction: EasingFunction = Easing.Linear.None;
	private _interpolationFunction = Interpolation.Linear;
	private _chainedTweens: Tween[] = [];
	private _onStartCallback: CallbackFunction = null;
	private _onStartCallbackFired = false;
	private _onUpdateCallback: CallbackFunction = null;
	private _onCompleteCallback: CallbackFunction = null;
	private _onStopCallback: CallbackFunction = null;

	constructor(object?: any) {
		this._object = object;
	}

	to(properties: any, duration: number): Tween {

		this._valuesEnd = properties;

		if (duration !== undefined) {
			this._duration = duration;
		}

		return this;

	};

	start(time?: number): Tween {

		Tween.add(this);

		this._isPlaying = true;

		this._onStartCallbackFired = false;

		this._startTime = time !== undefined ? time : Tween.Now();
		this._startTime += this._delayTime;

		for (var property in this._valuesEnd) {

			// Check if an Array was provided as property value
			if (this._valuesEnd[property] instanceof Array) {

				if (this._valuesEnd[property].length === 0) {
					continue;
				}

				// Create a local copy of the Array with the start value at the front
				this._valuesEnd[property] = [this._object[property]].concat(this._valuesEnd[property]);

			}

			// If `to()` specifies a property that doesn't exist in the source object,
			// we should not set that property in the object
			if (this._object[property] === undefined) {
				continue;
			}

			// Save the starting value.
			this._valuesStart[property] = this._object[property];

			if ((this._valuesStart[property] instanceof Array) === false) {
				this._valuesStart[property] *= 1.0; // Ensures we're using numbers, not strings
			}

			this._valuesStartRepeat[property] = this._valuesStart[property] || 0;

		}

		return this;

	};

	stop(): Tween {

		if (!this._isPlaying) {
			return this;
		}

		Tween.remove(this);
		this._isPlaying = false;

		if (this._onStopCallback !== null) {
			this._onStopCallback(this._object);
		}

		this.stopChainedTweens();
		return this;

	};

	end(): Tween {

		this.update(this._startTime + this._duration);
		return this;

	};

	stopChainedTweens() {

		for (var i = 0, numChainedTweens = this._chainedTweens.length; i < numChainedTweens; i++) {
			this._chainedTweens[i].stop();
		}

	};

	delay(amount: number): Tween {

		this._delayTime = amount;
		return this;

	};

	repeat(times: number): Tween {

		this._repeat = times;
		return this;

	};

	repeatDelay(amount: number): Tween {

		this._repeatDelayTime = amount;
		return this;

	};

	yoyo(enable: boolean): Tween {

		this._yoyo = enable;
		return this;

	};


	easing(easing: EasingFunction): Tween {

		this._easingFunction = easing;
		return this;

	};

	interpolation(interpolation) {

		this._interpolationFunction = interpolation;
		return this;

	};

	chain(...tweens: Tween[]) {

		this._chainedTweens = tweens;
		return this;

	};

	onStart(callback?: CallbackFunction) {

		this._onStartCallback = callback;
		return this;

	};

	onUpdate(callback?: CallbackFunction) {

		this._onUpdateCallback = callback;
		return this;

	};

	onComplete(callback?: CallbackFunction) {

		this._onCompleteCallback = callback;
		return this;

	};

	onStop(callback?: CallbackFunction) {

		this._onStopCallback = callback;
		return this;

	};

	update(time) {

		var property;
		var elapsed;
		var value;

		if (time < this._startTime) {
			return true;
		}

		if (this._onStartCallbackFired === false) {

			if (this._onStartCallback !== null) {
				this._onStartCallback(this._object);
			}

			this._onStartCallbackFired = true;
		}

		elapsed = (time - this._startTime) / this._duration;
		elapsed = elapsed > 1 ? 1 : elapsed;

		value = this._easingFunction(elapsed);

		for (property in this._valuesEnd) {

			// Don't update properties that do not exist in the source object
			if (this._valuesStart[property] === undefined) {
				continue;
			}

			var start = this._valuesStart[property] || 0;
			var end = this._valuesEnd[property];

			if (end instanceof Array) {

				this._object[property] = this._interpolationFunction(end, value);

			} else {

				// Parses relative end values with start as base (e.g.: +10, -3)
				if (typeof (end) === 'string') {

					if (end.charAt(0) === '+' || end.charAt(0) === '-') {
						end = start + parseFloat(end);
					} else {
						end = parseFloat(end);
					}
				}

				// Protect against non numeric properties.
				if (typeof (end) === 'number') {
					this._object[property] = start + (end - start) * value;
				}

			}

		}

		if (this._onUpdateCallback !== null) {
			this._onUpdateCallback(value);
		}

		if (elapsed === 1) {

			if (this._repeat > 0) {

				if (isFinite(this._repeat)) {
					this._repeat--;
				}

				// Reassign starting values, restart by making startTime = now
				for (property in this._valuesStartRepeat) {

					if (typeof (this._valuesEnd[property]) === 'string') {
						this._valuesStartRepeat[property] = this._valuesStartRepeat[property] + parseFloat(this._valuesEnd[property]);
					}

					if (this._yoyo) {
						var tmp = this._valuesStartRepeat[property];

						this._valuesStartRepeat[property] = this._valuesEnd[property];
						this._valuesEnd[property] = tmp;
					}

					this._valuesStart[property] = this._valuesStartRepeat[property];

				}

				if (this._yoyo) {
					this._reversed = !this._reversed;
				}

				if (this._repeatDelayTime !== undefined) {
					this._startTime = time + this._repeatDelayTime;
				} else {
					this._startTime = time + this._delayTime;
				}

				return true;

			} else {

				if (this._onCompleteCallback !== null) {

					this._onCompleteCallback(this._object);
				}

				for (var i = 0, numChainedTweens = this._chainedTweens.length; i < numChainedTweens; i++) {
					// Make the chained tweens start exactly at the time they should,
					// even if the `update()` method was called way past the duration of the tween
					this._chainedTweens[i].start(this._startTime + this._duration);
				}

				return false;

			}

		}
		return true;
	}

}

export type EasingFunction = (k: number) => number;

export class Easing {
	static Linear: { None: EasingFunction } = {

		None: (k) => {

			return k;

		}

	}

	static Quadratic = {

		In(k) {

			return k * k;

		},

		Out(k) {

			return k * (2 - k);

		},

		InOut(k) {

			if ((k *= 2) < 1) {
				return 0.5 * k * k;
			}

			return - 0.5 * (--k * (k - 2) - 1);

		}

	};

	static Cubic = {

		In(k) {

			return k * k * k;

		},

		Out(k) {

			return --k * k * k + 1;

		},

		InOut(k) {

			if ((k *= 2) < 1) {
				return 0.5 * k * k * k;
			}

			return 0.5 * ((k -= 2) * k * k + 2);

		}

	};

	static Quartic = {

		In(k) {

			return k * k * k * k;

		},

		Out(k) {

			return 1 - (--k * k * k * k);

		},

		InOut(k) {

			if ((k *= 2) < 1) {
				return 0.5 * k * k * k * k;
			}

			return - 0.5 * ((k -= 2) * k * k * k - 2);

		}

	};

	static Quintic = {

		In(k) {

			return k * k * k * k * k;

		},

		Out(k) {

			return --k * k * k * k * k + 1;

		},

		InOut(k) {

			if ((k *= 2) < 1) {
				return 0.5 * k * k * k * k * k;
			}

			return 0.5 * ((k -= 2) * k * k * k * k + 2);

		}

	}

	static Sinusoidal = {

		In(k) {

			return 1 - Math.cos(k * Math.PI / 2);

		},

		Out(k) {

			return Math.sin(k * Math.PI / 2);

		},

		InOut(k) {

			return 0.5 * (1 - Math.cos(Math.PI * k));

		}

	};

	static Exponential = {

		In(k) {

			return k === 0 ? 0 : Math.pow(1024, k - 1);

		},

		Out(k) {

			return k === 1 ? 1 : 1 - Math.pow(2, - 10 * k);

		},

		InOut(k) {

			if (k === 0) {
				return 0;
			}

			if (k === 1) {
				return 1;
			}

			if ((k *= 2) < 1) {
				return 0.5 * Math.pow(1024, k - 1);
			}

			return 0.5 * (- Math.pow(2, - 10 * (k - 1)) + 2);

		}

	};

	static Circular = {

		In(k) {

			return 1 - Math.sqrt(1 - k * k);

		},

		Out(k) {

			return Math.sqrt(1 - (--k * k));

		},

		InOut(k) {

			if ((k *= 2) < 1) {
				return - 0.5 * (Math.sqrt(1 - k * k) - 1);
			}

			return 0.5 * (Math.sqrt(1 - (k -= 2) * k) + 1);

		}

	};

	static Elastic = {

		In(k) {

			if (k === 0) {
				return 0;
			}

			if (k === 1) {
				return 1;
			}

			return -Math.pow(2, 10 * (k - 1)) * Math.sin((k - 1.1) * 5 * Math.PI);

		},

		Out(k) {

			if (k === 0) {
				return 0;
			}

			if (k === 1) {
				return 1;
			}

			return Math.pow(2, -10 * k) * Math.sin((k - 0.1) * 5 * Math.PI) + 1;

		},

		InOut(k) {

			if (k === 0) {
				return 0;
			}

			if (k === 1) {
				return 1;
			}

			k *= 2;

			if (k < 1) {
				return -0.5 * Math.pow(2, 10 * (k - 1)) * Math.sin((k - 1.1) * 5 * Math.PI);
			}

			return 0.5 * Math.pow(2, -10 * (k - 1)) * Math.sin((k - 1.1) * 5 * Math.PI) + 1;

		}

	};

	static Back = {

		In(k) {

			var s = 1.70158;

			return k * k * ((s + 1) * k - s);

		},

		Out(k) {

			var s = 1.70158;

			return --k * k * ((s + 1) * k + s) + 1;

		},

		InOut(k) {

			var s = 1.70158 * 1.525;

			if ((k *= 2) < 1) {
				return 0.5 * (k * k * ((s + 1) * k - s));
			}

			return 0.5 * ((k -= 2) * k * ((s + 1) * k + s) + 2);

		}

	};

	static Bounce = {

		In(k) {

			return 1 - Easing.Bounce.Out(1 - k);

		},

		Out(k) {

			if (k < (1 / 2.75)) {
				return 7.5625 * k * k;
			} else if (k < (2 / 2.75)) {
				return 7.5625 * (k -= (1.5 / 2.75)) * k + 0.75;
			} else if (k < (2.5 / 2.75)) {
				return 7.5625 * (k -= (2.25 / 2.75)) * k + 0.9375;
			} else {
				return 7.5625 * (k -= (2.625 / 2.75)) * k + 0.984375;
			}

		},

		InOut(k) {

			if (k < 0.5) {
				return Easing.Bounce.In(k * 2) * 0.5;
			}

			return Easing.Bounce.Out(k * 2 - 1) * 0.5 + 0.5;

		}

	}
}


export type InterpolationFunction = (v: number[], k: number) => number;

export class Interpolation {

	static Linear: InterpolationFunction = (v, k) => {

		var m = v.length - 1;
		var f = m * k;
		var i = Math.floor(f);
		var fn = Interpolation.Utils.Linear;

		if (k < 0) {
			return fn(v[0], v[1], f);
		}

		if (k > 1) {
			return fn(v[m], v[m - 1], m - f);
		}

		return fn(v[i], v[i + 1 > m ? m : i + 1], f - i);

	};

	static Bezier: InterpolationFunction = (v, k) => {

		var b = 0;
		var n = v.length - 1;
		var pw = Math.pow;
		var bn = Interpolation.Utils.Bernstein;

		for (var i = 0; i <= n; i++) {
			b += pw(1 - k, n - i) * pw(k, i) * v[i] * bn(n, i);
		}

		return b;

	}

	static CatmullRom: InterpolationFunction = (v, k) => {

		var m = v.length - 1;
		var f = m * k;
		var i = Math.floor(f);
		var fn = Interpolation.Utils.CatmullRom;

		if (v[0] === v[m]) {

			if (k < 0) {
				i = Math.floor(f = m * (1 + k));
			}

			return fn(v[(i - 1 + m) % m], v[i], v[(i + 1) % m], v[(i + 2) % m], f - i);

		} else {

			if (k < 0) {
				return v[0] - (fn(v[0], v[0], v[1], v[1], -f) - v[0]);
			}

			if (k > 1) {
				return v[m] - (fn(v[m], v[m], v[m - 1], v[m - 1], f - m) - v[m]);
			}

			return fn(v[i ? i - 1 : 0], v[i], v[m < i + 1 ? m : i + 1], v[m < i + 2 ? m : i + 2], f - i);

		}

	}

	static Utils = {

		Linear(p0: number, p1: number, t: number) {

			return (p1 - p0) * t + p0;

		},

		Bernstein(n: number, i: number) {

			var fc = Interpolation.Utils.Factorial;

			return fc(n) / fc(i) / fc(n - i);

		},

		Factorial: (n: number) => {
			var a = [1];


			var s = 1;

			if (a[n]) {
				return a[n];
			}

			for (var i = n; i > 1; i--) {
				s *= i;
			}

			a[n] = s;
			return s;

		},


		CatmullRom(p0, p1, p2, p3, t) {

			var v0 = (p2 - p0) * 0.5;
			var v1 = (p3 - p1) * 0.5;
			var t2 = t * t;
			var t3 = t * t2;

			return (2 * p1 - 2 * p2 + v0 + v1) * t3 + (- 3 * p1 + 3 * p2 - 2 * v0 - v1) * t2 + v0 * t + p1;

		}

	}
}

export type CallbackFunction = (object?: any) => void;