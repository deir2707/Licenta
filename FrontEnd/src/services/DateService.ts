import moment from "moment";

export interface IDate {
  currentDate: Date;
  formatDate: (date: Date | string) => string;
  dateToUTC: (date: Date) => Date;
}

export interface Duration {
  days: number;
  hours: number;
  minutes: number;
  seconds: number;
}

export class DateService implements IDate {
  currentDate: Date = new Date();

  formatDate = (
    date: string | Date | undefined,
    format: string = "L"
  ): string => {
    if (!date) return "";

    let dateObject = date;

    if (typeof date === "string") {
      dateObject = new Date(date);
    }

    const formatedDate = moment(dateObject).locale("en").format(format);
    return formatedDate;
  };

  ToTimezone = (date: Date, timezone: string): Date => {
    const dateObject = moment(date).utcOffset(timezone);
    return dateObject.toDate();
  };

  dateToUTC = (date: Date): Date => {
    return new Date(
      Date.UTC(date.getFullYear(), date.getMonth(), date.getDate())
    );
  };

  getDuration = (date: Date): Duration => {
    const duration = moment.duration(moment(date).diff(moment()));
    const days = Math.floor(duration.asDays());
    const hours = Math.floor(duration.asHours() % 24);
    const minutes = Math.floor(duration.asMinutes() % 60);
    const seconds = Math.floor(duration.asSeconds() % 60);

    return { days, hours, minutes, seconds };
  };

  durationToString = (duration: Duration): string => {
    const days = duration.days > 0 ? `${duration.days} days ` : "";
    const hours = duration.hours > 0 ? `${duration.hours} hours ` : "";
    const minutes = duration.minutes > 0 ? `${duration.minutes} minutes ` : "";
    const seconds =
      duration.seconds > 0 ? `and ${duration.seconds} seconds ` : "";

    return `${days}${hours}${minutes}${seconds}`;
  };
}

export const dateService = new DateService();
export default dateService;
