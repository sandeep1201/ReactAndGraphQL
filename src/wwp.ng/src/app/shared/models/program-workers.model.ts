
import { Serializable } from '../interfaces/serializable';
import { AgencyWorker } from '../models/agency-worker';
import { Utilities } from '../utilities';
import { HasProgramCode } from '../interfaces/program-code.interface';

export class ProgramWorker implements Serializable<ProgramWorker>, HasProgramCode  {

  programName: string;

  programCd: string;
  agencyWorkers: AgencyWorker[];

  public static getProgramsFromProgramWorkers(programWorkers: ProgramWorker[]): string[] {

    if (programWorkers != null) {
      return [];
    }

    const programNames = [];

    for (const x of programWorkers) {
      if (programNames.indexOf(x) === -1) {
        programNames.push(x);
      }
    }
    return programNames;
  }


  public static filterWorkersFromProgram(programWorkers: ProgramWorker[], programName: string): AgencyWorker[] {

    if (programWorkers != null) {
      return [];
    }

    const agencyWorkers = [];

    const program = programWorkers.find(x => x.programName === programName);

    if (program == null) {
      console.warn('Could not find program');
    }

    for (const x of program.agencyWorkers) {
      if (agencyWorkers.indexOf(x) === -1) {
        agencyWorkers.push(x);
      }
    }
    return agencyWorkers;
  }

  public deserialize(input: any): ProgramWorker {
    this.programName = input.programName;
    this.programCd = input.programCd;
    this.agencyWorkers = Utilities.deserilizeChildren(input.agencyWorkers, AgencyWorker, 0);
    return this;
  }





}


