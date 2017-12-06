import Action from 'Infrastructure/Action';
import * as DataService from './DataService';
import * as notifications from 'Components/Blocks/Notifications/Actions';
import { Invoice } from './models';

export const DASHBOARD_FETCHED_INVOICES = 'DASHBOARD_FETCHED_INVOICES';
export type DASHBOARD_FETCHED_INVOICES = { invoices: Invoice[] };
export function receiveLogin(invoices: Invoice[]): Action<DASHBOARD_FETCHED_INVOICES> {
  return {
    type: DASHBOARD_FETCHED_INVOICES,
    payload: {
      invoices: invoices
    }
  }
}