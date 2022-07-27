import { PageLayout } from "../components/PageLayout";

export interface Notification {
  id: string;
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
