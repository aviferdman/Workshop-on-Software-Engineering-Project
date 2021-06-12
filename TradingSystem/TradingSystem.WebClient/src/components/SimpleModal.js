import './SimpleModal.css'

const SimpleModal = ({ title, show, children, width, height, btn1Handle, btn1Text, btn1Class, btn2Handle, btn2Text, btn2Class }) => {
    if (!show) {
        return null;
    }

    let btnClass = 'simple-modal-buttons-props';
    btn1Class = btn1Class !== undefined ? `${btnClass} ${btn1Class}` : btnClass;
    btn2Class = btn2Class !== undefined ? `${btnClass} ${btn2Class}` : btnClass;
    return (
        <div className='simple-modal z-index-9999'>
            <section className='simple-modal-main' style={{
                width: width,
                height: height,
            }}>

                <div className="simple-modal-lines-props">
                    <h2 className='simple-modal-center-text'>{title}</h2>
                    {/*<div style={{ width: '100%' }} >*/}
                    {/*</div>*/}
                    {children}
                </div>


                <div className="simple-modal-buttons">
                    <button class={btn1Class} onClick={btn1Handle}> {btn1Text} </button>
                    <button class={btn2Class} onClick={btn2Handle}> {btn2Text} </button>
                </div>


            </section>
        </div>
    );
};

export default SimpleModal;
