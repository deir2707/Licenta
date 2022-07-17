import { PageLayout } from "../components/PageLayout";

export interface Notification {
  id: number;
  event: string;
  message: string;
  data: any;
}

export const MainPage = () => {
  return (
    <PageLayout>
      <div></div>
    </PageLayout>
  );
};
