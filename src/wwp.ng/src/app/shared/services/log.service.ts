// import { AngularFireDatabase } from 'angularfire2/database';
import { Injectable, NgZone } from '@angular/core';

import { Subscription, timer as TimerObservable } from 'rxjs';

import { Guid } from '../models/guid';
import { Dictionary } from '../models/keyed-collection';
import { version } from '../version';

export enum LogLevel {
  Trace = 0,
  Debug = 1,
  Info = 2,
  Warn = 3,
  Error = 4,
  Fatal = 5
}

const BAD_KEY = '__BAD_KEY__';

@Injectable()
export class LogServiceConfig {
  public eventsToConsole = false;
  public messagesToConsole = true;
  public heartbeatSeconds = 60;
}

/*
  - browser version
  - warnings in console (like Bootstrap)
  - URL (with user)
  - duration of time on URL
  - API times

  EVENTS:
  - startup
      - browser version
      - local time
      - IP address
  - page load (ngInit)
  -
*/

@Injectable()
export class LogService {
  private config: LogServiceConfig;
  private sessionId: Guid;
  private perfDictionary: Dictionary<number>;
  private eventKeyDictionary: Dictionary<string>;
  private sidKey: string;
  private timerSub: Subscription;

  constructor(private zone: NgZone, cnfg?: LogServiceConfig) {
    // Check for config.
    if (cnfg == null) {
      this.config = new LogServiceConfig();
    } else {
      this.config = cnfg;
    }

    // Start off by creating a new unique session ID.
    this.sessionId = new Guid();
    this.perfDictionary = new Dictionary<number>();
    this.eventKeyDictionary = new Dictionary<string>();
    // // this.afDb.database.goOnline();
    // const listRef = this.afDb.list('/sids');
    // const entry = { sid: this.sessionId.toString(), date: new Date().toJSON(), host: window.location.host, v: version.toString() };
    // this.sidKey = listRef.push(entry).key;
    // // this.afDb.database.goOffline();
    this.zone.runOutsideAngular(() => {
      this.initHeartbeat();
    });
  }

  /**
   * Call the disose of the service if you want to get rid of it prematurely.
   */
  public dispose(): void {
    if (this.timerSub != null) {
      this.timerSub.unsubscribe();
    }
  }

  public log(level: LogLevel, message: string) {
    try {
      // // this.afDb.database.goOnline();
      // const logsRef = this.afDb.list('/logs');
      const now = new Date().toJSON();
      // const levelUpper = LogLevel[level].toUpperCase();

      // const logEntry = { date: now, level: levelUpper, sid: this.sessionId.toString(), message: message };
      // logsRef.push(logEntry);
      // // this.afDb.database.goOffline();

      if (this.config.messagesToConsole) {
        console.log(`${level} | ${now} | ${this.sessionId} | ${message}`);
      }
    } catch (error) {
      // Eat the error.
      console.error(error);
    }
  }

  private initHeartbeat() {
    // // Create a timer to start in 60 seconds and fire every 60 seconds after that.
    // const delay: number = this.config.heartbeatSeconds * 1000;
    // const timer = TimerObservable.create(0, delay);
    // this.timerSub = timer.subscribe(t => {
    //   const now = new Date().toJSON();
    //   // this.afDb.database.goOnline();
    //   const objRef = this.afDb.object(`/sids/${this.sidKey}`);
    //   objRef.update({ lastOnline: now });
    //   // this.afDb.database.goOffline();
    // });
  }

  public sessionUser(user: string) {
    // // this.afDb.database.goOnline();
    // if (user != null && user !== '') {
    //   const objRef = this.afDb.object(`/sids/${this.sidKey}`);
    //   objRef.update({ user: user });
    // }
    // // this.afDb.database.goOffline();
  }

  public event(event: string, details?: any, timeInMs?: number): void {
    // let key: string;

    // try {
    //   // this.afDb.database.goOnline();
    //   const listRef = this.afDb.list('/events');
    //   const now = new Date().toJSON();

    //   if (timeInMs) {
    //     timeInMs = +timeInMs.toFixed(2);
    //     if (details != null) {
    //       const logEntry = { date: now, event: event, sid: this.sessionId.toString(), ms: timeInMs, details: details };
    //       key = listRef.push(logEntry).key;

    //       if (this.config.eventsToConsole) {
    //         console.log(`EVENT | ${now} | ${this.sessionId} | ${event} | ${timeInMs} | ${JSON.stringify(details)}`);
    //       }
    //     } else {
    //       const logEntry = { date: now, event: event, sid: this.sessionId.toString(), ms: timeInMs };
    //       key = listRef.push(logEntry).key;

    //       if (this.config.eventsToConsole) {
    //         console.log(`EVENT | ${now} | ${this.sessionId} | ${event} | ${timeInMs}`);
    //       }
    //     }
    //   } else if (details) {
    //     const logEntry = { date: now, event: event, sid: this.sessionId.toString(), details: details };
    //     key = listRef.push(logEntry).key;

    //     if (this.config.eventsToConsole) {
    //       console.log(`EVENT | ${now} | ${this.sessionId} | ${event} | ${JSON.stringify(details)}`);
    //     }
    //   } else {
    //     const logEntry = { date: now, event: event, sid: this.sessionId.toString() };
    //     key = listRef.push(logEntry).key;

    //     if (this.config.eventsToConsole) {
    //       console.log(`EVENT | ${now} | ${this.sessionId} | ${event}`);
    //     }
    //   }

    //   // this.afDb.database.goOffline();
    // } catch (error) {
    //   // Eat the error.
    //   key = BAD_KEY;
    //   console.error(error);
    // }
    // return key;
  }

  public error(message: string): void {
    this.log(LogLevel.Error, message);
  }

  public info(message: string): void {
    this.log(LogLevel.Info, message);
  }

  public trace(message: string): void {
    this.log(LogLevel.Trace, message);
  }

  public debug(message: string): void {
    this.log(LogLevel.Debug, message);
  }

  public warn(message: string): void {
    this.log(LogLevel.Warn, message);
  }

  public fatal(message: string): void {
    this.log(LogLevel.Fatal, message);
  }

  public timerStart(event: string, doLogEvent = false) {
    // if (this.perfDictionary.containsKey(event)) {
    //   // Since this is a "duplicate" event that is already being tracked we'll
    //   // log a warning.
    //   this.warn(`Timer '${event}' started again, ignoring.`);
    // } else {
    //   this.perfDictionary.add(event, performance.now());
    //   if (doLogEvent === true) {
    //     this.trace(`Timer '${event}' started.`);
    //   }
    // }
  }

  public timerStartEvent(event: string, details?: any) {
    // if (this.perfDictionary.containsKey(event)) {
    //   // Since this is a "duplicate" event that is already being tracked we'll
    //   // log a warning.
    //   this.warn(`Timer '${event}' started again, ignoring.`);
    // } else {
    //   const key = this.event(event, details);
    //   this.perfDictionary.add(event, performance.now());
    //   this.eventKeyDictionary.add(event, key);
    // }
  }

  public timerEndEvent(event: string, details?: any) {
    // if (this.perfDictionary.containsKey(event)) {
    //   const end = performance.now();
    //   const start = this.perfDictionary.item(event);
    //   this.event(event, details, end - start);
    //   this.perfDictionary.remove(event);
    // } else {
    //   this.warn(`Event '${event}' end called but wasn't started.`);
    // }
  }

  public timerEndUpdateEvent(event: string) {
    // try {
    //   if (this.perfDictionary.containsKey(event)) {
    //     const end = performance.now();
    //     const start = this.perfDictionary.item(event);

    //     // We should have already logged the event and just need to update it
    //     // with the timing info.
    //     if (this.eventKeyDictionary.containsKey(event)) {
    //       // Find the event from the saved key.
    //       const key = this.eventKeyDictionary.item(event);

    //       // We might have a bad key.
    //       if (key !== BAD_KEY) {
    //         // this.afDb.database.goOnline();
    //         const objRef = this.afDb.object(`/events/${key}`);
    //         const timeInMs = +(end - start).toFixed(2);

    //         objRef.update({ ms: timeInMs });
    //         // this.afDb.database.goOffline();
    //       }

    //       this.eventKeyDictionary.remove(event);
    //     } else {
    //       this.warn(`Event '${event}' end update called but key doesn't exist.`);
    //     }

    //     this.perfDictionary.remove(event);
    //   } else {
    //     this.warn(`Event '${event}' end update called but wasn't started.`);
    //   }
    // } catch (error) {
    //   // Eat the error.
    //   console.error(error);
    // }
  }

  public timerEnd(event: string) {
    // if (this.perfDictionary.containsKey(event)) {
    //   this.perfDictionary.remove(event);
    // }
  }

  public timerEndLog(event: string) {
    // try {
    //   if (this.perfDictionary.containsKey(event)) {
    //     const end = performance.now();
    //     const start = this.perfDictionary.item(event);

    //     const timeInMs = +(end - start).toFixed(2);

    //     this.trace(`Timer '${event}' in ${timeInMs} ms.`);

    //     this.perfDictionary.remove(event);
    //   } else {
    //     this.warn(`Timer '${event}' end log called but wasn't started.`);
    //   }
    // } catch (error) {
    //   // Eat the error.
    //   console.error(error);
    // }
  }
}
