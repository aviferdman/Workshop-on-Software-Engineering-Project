import {makeStyles} from "@material-ui/core";

const useFormsStyles = makeStyles((theme) => ({
    center: {
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        height: '100vh'
    },
    form: {
        border: '2px solid gray',
        padding: theme.spacing(1)
    },
    title: {
        fontSize: '1.5rem',
        marginTop: '0px',
        marginBottom: theme.spacing(2),
        alignSelf: 'flex-start'
    }
}));
export default useFormsStyles;