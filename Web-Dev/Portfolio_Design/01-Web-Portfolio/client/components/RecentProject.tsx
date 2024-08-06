/* eslint-disable @next/next/no-img-element */
import { projects } from '@/data/project'
import React from 'react'
import { PinContainer } from './ui/3d-pin'
import { FaLocationArrow } from 'react-icons/fa6'

const RecentProject = () => {
    return (
        <section className='py-20' id='projects'>
            <h1 className='heading'>
                A small selection of {' '}
                <span className='text-purple'>
                    recent project
                </span>
            </h1>
            <div className='flex flex-wrap items-center justify-center p-4 gap-x-24 gap-y-8 mt-10'>
                {projects.map(({id, title, des, img, iconName, iconLists, link }) =>
                    <div key={id} className='sm:h-[41rem] lg:min-h-[32.5rem] h-[32rem] flex items-center justify-center sm:w-[570px] w-[80vw]'>
                        <PinContainer title={link} href={link}>
                            <div className='relative flex justify-center items-center sm:w-[570px] w-[80vw] overflow-hidden sm:h-[30vh] h-[30vh] mb-0'>
                                <div className='relative w-full h-full overflow-hidden lg:rounded-3xl rounded-3xl'
                                    style={{ backgroundColor: "#13162D" }}
                                >
                                    <img src="/bg.png" alt="bg-img" />
                                </div>
                                <img src={img} alt={title}
                                    className='z-10 absolute bottom-0 overflow-hidden'
                                />
                            </div>
                            <h1 className='font-bold lg:text-2xl md:text-xl text-base line-clamp-1 mt-5'>
                                {title}
                            </h1>
                            <p className='lg:text-xl lg:font-normal font-light text-sm line-clamp-2'>
                                {des}
                            </p>

                            <div className='flex items-center justify-evenly mt-7 mb-3'>
                                <div className='flex items-center'>
                                    {iconLists.map((icon, index) => (
                                        <div key={icon} className='border border-white/[0.2] rounded-full bg-black lg:w-10 lg:h-10 w-8 h-8 flex justify-center items-center'
                                            style={{ transform: `translateX(-${5 * index * 2}px` }}
                                            title={iconName ? iconName[index] : ''}
                                        >
                                            <img src={icon} alt={icon} className='p-2'/>
                                        </div>
                                    ))}
                                </div>
                                <div className='flex justify-center items-center'>
                                    <p className='flex lg:text-xl md:text-xs text-sm text-purple'>
                                        Check Live Site
                                    </p>
                                    <FaLocationArrow className='ms-3' color='#CBACF9'/>
                                </div>
                            </div>
                        </PinContainer>
                </div>)}
            </div>
        </section>
    )
}

export default RecentProject