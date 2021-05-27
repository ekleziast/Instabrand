import { HomeProvider } from 'context/home';
import Home from 'components/Home/index';
import authMiddleware from 'middlewares/auth';
import Values from 'classes/Values';

export default function Main() {
    return (
        <HomeProvider>
            <Home/>
        </HomeProvider>
    );
}

export async function getServerSideProps({ req, res }) {
    await authMiddleware({ req, res });

    return Values.emptyProps;
}